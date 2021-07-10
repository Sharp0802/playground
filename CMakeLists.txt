cmake_minimum_required(VERSION 3.0)
project(probo)

set(CMAKE_AUTOGEN_VERBOSE true)
set(CMAKE_VERBOSE_MAKEFILE false)

set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${PROJECT_SOURCE_DIR}/bin)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${PROJECT_SOURCE_DIR}/bin)
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${PROJECT_SOURCE_DIR}/bin)

set(CMAKE_C_COMPILER avr-gcc)
set(CMAKE_CXX_COMPILER avr-g++)

add_compile_options(-Os -fomit-frame-pointer -fexceptions -mno-int8 -Wall --std=gnu++2a)

add_compile_options(-mmcu=atmega8a)
add_link_options(-mmcu=atmega8a)

include_directories(inc/)
include_directories(lib/)
link_libraries(lib/libapi.a)

add_library(probo STATIC

        src/probo.cpp
        src/component/audio.cpp
        src/component/diagnostics.cpp
        src/component/eeprom.cpp
        src/component/input.cpp
        src/component/internal.cpp
        src/component/lifetime.cpp
        src/component/threading.cpp

        inc/probo.hpp
        inc/component/audio.hpp
        inc/component/diagnostics.hpp
        inc/component/eeprom.hpp
        inc/component/input.hpp
        inc/component/internal.hpp
        inc/component/lamp.hpp
        inc/component/lifetime.hpp
        inc/component/motor.hpp
        inc/component/threading.hpp)