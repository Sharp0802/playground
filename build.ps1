# set location to build directory
$pre = $PWD
set-location "$(split-path -parent $MyInvocation.MyCommand.Definition)"
set-location build

# make $env:PATH to use msys2
$pre_path = $env:PATH
$env:PATH = ($env:PATH.split(';') | where-object { $_ -like "*msys*" }) + ($env:PATH) -join ';'

# build
cmake CMakeLists.txt -G"MinGW Makefiles"
mingw32-make

# clean $env:PATH
$env:PATH = $pre_path

# clean directory
set-location "$pre"