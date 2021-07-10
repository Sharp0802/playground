
#ifndef INPUT_HPP
#define INPUT_HPP

#include "internal.hpp"

#define PURE __attribute__((pure))
#define CONST __attribute__((const))
#define NODISCARD [[nodiscard]]

using namespace Probo::Internal;

namespace Probo::Input
{
	struct OnBoardInputStatus
	{
	private:
		unsigned char Data;
	public:
		explicit OnBoardInputStatus(unsigned char data);

		NODISCARD bool Current() const;

		NODISCARD bool RisingEdge() const;

		NODISCARD bool FallingEdge() const;

		NODISCARD bool BothEdge() const;
	};

	template<const unsigned char port>
	class OnBoardInput
	{
	public:
		static OnBoardInputStatus GetStatus()
		{
			return OnBoardInputStatus(
					static_cast<unsigned char>(
							(GetInputData(port) != 0) << 0 |
							(GetRisingEdge(port) != 0) << 1 |
							(GetFallingEdge(port) != 0) << 2 |
							(GetBothEdge(port) != 0) << 3));
		}
	};

	enum class DirectionFlag : unsigned char
	{
		Up    = 0b0001,
		Down  = 0b0010,
		Left  = 0b0100,
		Right = 0b1000
	};

	class RemoteInput
	{
		static DirectionFlag GetLeft();

		static DirectionFlag GetRight();

		static DirectionFlag GetFront();
	};
}

#undef PURE
#undef CONST
#undef NODISCARD

#endif //INPUT_HPP
