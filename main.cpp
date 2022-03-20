#include <iostream>

#include "com.hpp"
#include "proc.hpp"

int main()
{
	std::cout << proc::exec("echo 'Hello World!'");
    return 0;
}
