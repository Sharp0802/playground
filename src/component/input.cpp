#include "component/input.hpp"

using namespace Probo::Input;

OnBoardInputStatus::OnBoardInputStatus(unsigned char data) : Data(data) {}

bool OnBoardInputStatus::Current() const
{
	return Data & 0b1;
}

bool OnBoardInputStatus::RisingEdge() const
{
	return Data & 0b10;
}

bool OnBoardInputStatus::FallingEdge() const
{
	return Data & 0b100;
}

bool OnBoardInputStatus::BothEdge() const
{
	return Data & 0b1000;
}

DirectionFlag RemoteInput::GetLeft()
{
	return static_cast<DirectionFlag>(
			((GetRemoteDigitalData(1) != 0) << 0) |
			((GetRemoteDigitalData(2) != 0) << 1) |
			((GetRemoteDigitalData(3) != 0) << 2) |
			((GetRemoteDigitalData(4) != 0) << 3));
}

DirectionFlag RemoteInput::GetRight()
{
	return static_cast<DirectionFlag>(
			((GetRemoteDigitalData(5) != 0) << 0) |
			((GetRemoteDigitalData(6) != 0) << 1) |
			((GetRemoteDigitalData(7) != 0) << 2) |
			((GetRemoteDigitalData(8) != 0) << 3));
}

DirectionFlag RemoteInput::GetFront()
{
	return static_cast<DirectionFlag>(
			((GetRemoteFrontData(1) != 0) ? 0b0110 : 0b0) | //LB
			((GetRemoteFrontData(2) != 0) ? 0b0101 : 0b0) | //RB
			((GetRemoteFrontData(3) != 0) ? 0b1010 : 0b0) | //LF
			((GetRemoteFrontData(4) != 0) ? 0b1001 : 0b0)); //RF
}
