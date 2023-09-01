#include "com.hpp"

#define lock(mutex_var) std::scoped_lock<std::mutex> lock(mutex_var);

using namespace std::this_thread;
using namespace std::chrono;
using namespace std::chrono_literals;

void __attribute((always_inline)) ensure(int res, const std::string& str)
{
	if (res < 0)
		throw std::runtime_error(str);
}

com::com(int port_number) : port_number(port_number)
{

}

com::~com()
{
	close();
}

void com::open(const int port_number, int* __restrict handle, termios* __restrict pre_open_state)
{
	std::stringstream com_name_stream;
	com_name_stream << "/dev/ttyUSB" << port_number;
	const auto com_name = com_name_stream.str();

	*handle = ::open(com_name.c_str(), O_RDWR | O_NOCTTY);
	ensure(*handle, "failed to open com port");

	int res;

	res = ioctl(*handle, TCFLSH, 2);
	ensure(res, "failed to purge com port");
	res = tcgetattr(*handle, pre_open_state);
	ensure(res, "failed to store com state");

	struct termios master = *pre_open_state;

	res = cfsetspeed(&master, (speed_t) 57600);
	ensure(res, "failed to set bandwidth");
	master.c_cflag |= PARODD; // use odd parity
	master.c_cflag |= CS8; // byte size := 8
	master.c_cflag &= ~CSTOPB; // cnt of stop bit := 1

	res = tcsetattr(*handle, TCSANOW, &master);
	ensure(res, "failed to apply com state");
}

std::vector<com_info> com::get_com_ports()
{
	constexpr auto devices = "/sys/class/tty";

	std::vector<com_info> infos;

	for (const auto& entry : fs::directory_iterator(devices))
	{
		const auto& path = entry.path();
		const auto& str_entry = path.filename().string();
		if (str_entry.starts_with("ttyUSB"))
		{
			const auto& port_number_path = path.string().append("/device/port_number");

			std::ifstream stream;
			stream.open(port_number_path, std::ios_base::in);
			std::stringstream stream_str;
			stream_str << stream.rdbuf();

			const auto port_number = std::strtol(stream_str.str().c_str(), nullptr, 10);

			infos.emplace_back(str_entry, static_cast<int>(port_number));
		}
	}

	return infos;
}

void com::open()
{
	lock(lock_handle)
	{
		if (opened)
			throw std::runtime_error("the com port is already opened");
		try
		{
			open(port_number, &com_handle, &com_pre_open_state);
		}
		catch (const std::exception& exception)
		{
			std::stringstream stream;
			stream << "error occurred: " << exception.what() << "(errno: " << errno << ")";
			throw std::runtime_error(stream.str());
		}
		sleep_for(sleep_duration);
		opened = true;
	}
}

void com::close()
{
	lock(lock_handle)
	{
		if (!opened)
			return;
		tcsetattr(com_handle, TCSANOW, &com_pre_open_state);
		sleep_for(sleep_duration);
		::close(com_handle);
		opened = false;
	}
}
