
#include "component/lifetime.hpp"
#include "component/internal.hpp"

using namespace Probo::Lifetime;

int MainInitializer::Activate()
{
    Init();
    start();
    Main();
    end();
    Dispose();
    return 0;
}

MainInitializer::MainInitializer(void (*init)(), void (*main)(), void (*dispose)())
        : Init(init), Main(main), Dispose(dispose)
{
    Activate();
}
