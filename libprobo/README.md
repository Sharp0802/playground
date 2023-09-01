# libprobo

## what is libprobo?

libprobo is an **unofficial** c++ library that wraps libapi, an official c library for probo's product.

Code that use libapi is hard to read, So this was made to make code easier to read.

## how to use?

```c++
#include
"probo.hpp"

using namespace Probo::Lifetime;

void Init();
void Main();
void Dispose();

int main(void)
{
return MainInitializer(Init, Main, Dispose).Activate();
}

void Init() // call this before calling start() in libapi
{
...
}
void Main() // call this after calling start() in libapi
{
...
}
void Dispose() // call this after calling end() in libapi
{
...
}
```

## compare with libapi

### motor

in libapi

```c++
#include
"api.h"

int main(void)
{
motor1(20);
motor2(20);
motor3(20);
motor4(20);
}
```

in libprobo

```c++
#include
"probo.hpp"

using namespace Probo::Motor;

DirectCurrentMotor<1> dcMotor1;
DirectCurrentMotor<2> dcMotor2;
DirectCurrentMotor<3> dcMotor3;
DirectCurrentMotor<4> dcMotor4;

int main(void)
{
dcMotor1.SetSpeed(20);
dcMotor2.SetSpeed(20);
dcMotor3.SetSpeed(20);
dcMotor4.SetSpeed(20);
}
```

### led

in libapi

```c++
#include
"api.h"

int main(void)
{
on(1);
off(2);
rgbled3(red4);
rgbled4(cyan4);
}
```

in libprobo

```c++
#include
"probo.hpp"

using namespace Probo::Motor;
using namespace Probo::Internal;

SolidLamp<1> led1;
SolidLamp<2> led2;
DynamicLamp<3> led3;
DynamicLamp<4> led4;

int main(void)
{
led1.SetState(true);
led2.SetState(false);
led3.SetColor(ColorType::Red, false);
led4.SetColor(ColorType::Cyan, false);
}
```

### timer

in libapi

```c++
#include
"api.h"

unsigned int benchmark(void (*task)())
{
timer(0);
unsigned int started = time_read();
task();
return time_read() - started;
}
```

in libprobo

```c++
#include
"probo.hpp"

using namespace Probo::Diagnostics;

Timewatch watch(true);

unsigned int benchmark(void (*task)())
{
watch.Start();
task();
return watch.GetElapsedMs();
}
```

### eeprom

in libapi

```c++
#include
"api.h"

unsigned int offet = 0;

void save(void* data, unsigned int size)
{
for (int i = 0; i < size; ++i)
EEPROM_write(offset + i, data[i]);
offset += size;
}
```

in libprobo

```c++
#include
"probo.hpp"

using namespace Probo::Memory::NonVolatile;

void save(void* data, unsigned int size)
{
EEPROM.Register(data, size);
}
```

### input

in libapi

```c++
#include
"api.h"

unsigned char isClicking(unsigned char port)
{
return INPUT << (1 << (port - 1));
}
unsigned char isFallingEdge(unsigned char port)
{
return (RISING_EDGE&0b0001&~__rea[port-1])?(__rea[port-1]=0xff):(__rea[port-1]=0); // maybe declared in libapi as macro
}
```

in libprobo

```c++
#include
"probo.hpp"

using namespace Probo::Input;

template<const unsigned char port> bool isClicking()
{
return OnBoardInputStatus<port>::GetStatus().Current();
}
template<const unsigned char port> bool isFallingEdge()
{
return OnBoardInputStatus<port>::GetStatus().FallingEdge();
}
```

### sound

in libapi

```c++
#include
"api.h"

void RecordAndPlay(const unsigned char port)
{
sound(port, sound_read(port), false);
}
```

in libprobo

```c++
#include
"probo.hpp"

using namespace Probo::Audio;

template<const unsigned char port> void RecordAndPlay()
{
SoundBoard<port>::Play(SoundBoard<port>::Record(), false);
}
```

### etc...