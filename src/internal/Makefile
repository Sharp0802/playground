
NAME := origin-demangler.wasm

CC := clang
FLAGS :=  --sysroot=/usr/share/wasi-sysroot/ --target=wasm32-unknown-wasi -Oz

LDFLAGS := $(FLAGS)
CFLAGS :=  $(FLAGS) -emit-llvm -I. 

TARGETS := $(wildcard *.c)
OBJECTS := $(TARGETS:.c=.o)

all: $(OBJECTS)
	$(CC) $(LDFLAGS) $^ -o $(NAME)

%.o: %.c
	$(CC) $(CFLAGS) -c $< -o $@

clean:
	rm -f *.o

.PHONY: all clean