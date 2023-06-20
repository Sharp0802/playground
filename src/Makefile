
NAME := origin-demangler.wasm

CC := clang
FLAGS :=  --sysroot=/usr/share/wasi-sysroot/ --target=wasm32-unknown-wasi -Oz

LDFLAGS := $(FLAGS)
CFLAGS :=  $(FLAGS) -emit-llvm -I. 

TARGETS := $(wildcard *.c)
OBJECTS := $(TARGETS:.c=.o)

all: $(OBJECTS)
	$(CC) $(LDFLAGS) $(addprefix tmp/,$^) -o ../res/$(NAME)

%.o: %.c
	mkdir -p tmp
	$(CC) $(CFLAGS) -c $< -o tmp/$@	

clean:
	rm -rf tmp

.PHONY: all clean
