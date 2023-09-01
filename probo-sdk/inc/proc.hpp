#pragma once

#include <string>

class proc
{
public:
	static std::string exec(const std::string& cmd);

	static void avrdude(int port, const std::string& file);
};
