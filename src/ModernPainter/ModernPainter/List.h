#pragma once

#include "framework.h"
#include "SpinLock.h"


template<class T>
class List
{
public:
	const size_t DefaultCapacity = 8;

private:
	T* _buffer;
	size_t _capacity;
	SpinLock _lock;

public:
	size_t Size;

public:
	List();
	List(size_t capacity);
	~List();

	List(const List&) = delete;
	List& operator =(const List&) = delete;

public:
	size_t Add(const T& value);
	bool RemoveAt(const size_t at, T* output);
	T& operator[](const size_t i);

	T* begin();
	T* end();
};

template<class T>
inline List<T>::List() :
	_capacity(DefaultCapacity),
	Size(0)
{
	_buffer = reinterpret_cast<T*>(malloc(sizeof(T) * DefaultCapacity));
}

template<class T>
inline List<T>::List(size_t capacity) :
	_capacity(capacity),
	Size(0)
{
	_buffer = reinterpret_cast<T*>(malloc(sizeof(T) * capacity));
}

template<class T>
inline List<T>::~List()
{
	free(_buffer);
	Size = 0;
	_capacity = 0;
}

template<class T>
inline size_t List<T>::Add(const T& value)
{
	_lock.Lock();
	Size++;
	if (Size > _capacity)
	{
		_capacity *= 2;
		realloc(_buffer, sizeof(T) * _capacity);
	}
	size_t idx = Size - 1;
	_buffer[idx] = value;
	_lock.Unlock();
	return idx;
}

template<class T>
inline bool List<T>::RemoveAt(const size_t at, T* output)
{
	_lock.Lock();
	bool ret;
	if (Size <= at)
	{
		ret = false;
	}
	else
	{
		if (output != nullptr) *output = _buffer[at];
		memmove(_buffer + at, _buffer + at + 1, Size - at - 1);
		ret = true;
	}
	_lock.Unlock();
	return ret;
}

template<class T>
inline T& List<T>::operator[](const size_t i)
{
	return _buffer[i];
}

template<class T>
inline T* List<T>::begin()
{
	return _buffer;
}

template<class T>
inline T* List<T>::end()
{
	return _buffer + Size;
}
