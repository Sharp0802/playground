#ifndef PROBO_H
#define PROBO_H

#   if __cplusplus

#       include "component/lamp.hpp"
#       include "component/motor.hpp"
#       include "component/audio.hpp"
#       include "component/diagnostics.hpp"
#       include "component/eeprom.hpp"
#       include "component/internal.hpp"

#   else

#       warning This library was made for only cpp projs, not c proj.

#   endif

#endif
