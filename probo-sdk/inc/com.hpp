#pragma once

#include <sys/ioctl.h>
#include <chrono>
#include <fcntl.h>
#include <filesystem>
#include <string>
#include <sstream>
#include <stdexcept>
#include <termios.h>
#include <thread>
#include <mutex>
#include <utility>
#include <vector>
#include <fstream>

namespace fs = std::filesystem;

namespace
{
	using namespace std::chrono_literals;

	constexpr auto sleep_duration = 60ms;
}

struct com_info
{
	explicit com_info(std::string name, int port_number) : name(std::move(name)), port_number(port_number)
	{

	}

	const std::string name;
	const int port_number;
};

class com
{
private:
	std::mutex lock_handle{};
	bool opened = false;

	int com_handle = -1;
	struct termios com_pre_open_state{};

	const int port_number;

private:
	static void open(int port_number, int* handle, termios* pre_open_state);

	static std::vector<com_info> get_com_ports();

public:
	explicit com(int port_number);

	~com();

	void open();

	void close();

public:
	com(const com&) = delete;
	com& operator= (const com&) = delete;
};
