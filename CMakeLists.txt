cmake_minimum_required(VERSION 3.23)
project(DelaunayTriangulation)

set(CMAKE_CXX_STANDARD 23)
set(CMAKE_VERBOSE_MAKEFILE true)
set(CMAKE_AUTOGEN_VERBOSE true)

find_package(OpenGL REQUIRED)
find_package(GLUT REQUIRED)
find_package(glfw3 REQUIRED)
find_package(GLEW REQUIRED)

add_compile_options(-O2 -march=native)

add_executable(DelaunayTriangulation src/main.cpp)

target_link_libraries(DelaunayTriangulation ${OPENGL_LIBRARIES} ${GLUT_LIBRARY} glfw GLEW::GLEW)