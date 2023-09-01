cmake_minimum_required(VERSION 3.21)
project(probo_sdk)

set(CMAKE_CXX_STANDARD 23)

include_directories(./inc)

add_compile_options(-Wall -O2)

add_executable(probo_sdk main.cpp src/com.cpp inc/com.hpp src/proc.cpp inc/proc.hpp)
