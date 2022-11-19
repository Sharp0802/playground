#pragma once

#include <vector>
#include <functional>
#include "framework.h"

template<class... ArgsT>
class EventHandler
{
private:
	using HandlerT = std::function<void(void*, ArgsT...)>;

	void* _self;
	HandlerT _handler;
	LONG _id;

public:
	EventHandler(void* self, HandlerT handler) :
		_self(self),
		_handler(handler)
	{
		static volatile ULONG _sId;
		_id = _InterlockedIncrement(&_sId);
	}

public:
	void operator()(ArgsT... params) const 
	{
		if (_handler) _handler(_self, params...);
	}

	bool operator==(const EventHandler& rhs) const
	{
		return _id == rhs._id;
	}
};

template<typename... ArgsT>
class EventSource
{
private:
	std::vector<EventHandler<ArgsT...>> _handlers;

public:
	void operator()(ArgsT... params)
	{
		for (auto& handler : _handlers)
			handler(params...);
	}

	void operator+=(EventHandler<ArgsT...>& handler)
	{
		_handlers.emplace_back(handler);
	}

	void operator-=(EventHandler<ArgsT...>& handler)
	{
		std::_Erase_remove(_handlers, handler);
	}
};
