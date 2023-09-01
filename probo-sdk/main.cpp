#include <iostream>

#include "com.hpp"
#include "proc.hpp"

int main()
{
	com port(0);
	port.open();
	proc::avrdude(0, "main.hex");
	port.close();
    return 0;
}
