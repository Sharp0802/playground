cmake_minimum_required(VERSION 3.20)
project(lwman LANGUAGES C)

set(CMAKE_BUILD_TYPE Release)

file(GLOB SRCs ../src/*.c)
include_directories(../inc)

add_executable(lwman WIN32 ${SRCs})

set_target_properties(lwman PROPERTIES
    
    C_STANDARD 99

    ARCHIVE_OUTPUT_DIRECTORY ../bin
    LIBRARY_OUTPUT_DIRECTORY ../bin
    RUNTIME_OUTPUT_DIRECTORY ../bin)

target_compile_options(lwman PRIVATE -Wall -Wextra -O2 -finput-charset=UTF-8 -mwindows)
