#pragma once

#include "framework.h"
#include "List.h"


template<class KeyT, class ValueT>
class MapNode
{
public:
	KeyT Key;
	ValueT Value;
	MapNode<KeyT, ValueT>* LVal;
	MapNode<KeyT, ValueT>* RVal;

public:
	MapNode(const KeyT& key, const ValueT& value) :
		Key(key),
		Value(value),
		LVal(nullptr),
		RVal(nullptr)
	{
	}

	~MapNode()
	{
		if (LVal != nullptr)
			delete LVal;
		if (RVal != nullptr)
			delete RVal;
	}

public:
	bool ContainsKey(const KeyT& key);
	bool ContainsValue(const ValueT& value);
	void GetAll(List<ValueT*>* vec);
	bool TryGet(const KeyT& key, ValueT* value);
	bool TryAdd(const KeyT& key, const ValueT& value);
};

template<class KeyT, class ValueT>
class Map
{
private:
	MapNode<KeyT, ValueT>* root;

public:
	Map();
	~Map();

	Map(const Map&) = delete;
	Map& operator =(const Map&) = delete;

public:
	bool ContainsKey(_In_ const KeyT& key);
	bool ContainsValue(_In_ const ValueT& value);
	void GetAll(_Inout_z_ List<ValueT*>* vec);
	bool TryGet(_In_ const KeyT& key, _Outptr_opt_ ValueT* value);
	bool TryAdd(_In_ const KeyT& key, _In_ const ValueT& value);
};

template<class KeyT, class ValueT>
inline bool MapNode<KeyT, ValueT>::ContainsKey(const KeyT& key)
{
	return 
		Key == key || 
		(Key < key 
			? (RVal != nullptr && RVal->ContainsKey(key))
			: (LVal != nullptr && LVal->ContainsKey(key)));
}

template<class KeyT, class ValueT>
inline bool MapNode<KeyT, ValueT>::ContainsValue(const ValueT& value)
{
	return 
		Value == value || 
		(RVal != nullptr && RVal->ContainsValue(value)) || 
		(LVal != nullptr && LVal->ContainsValue(value));
}

template<class KeyT, class ValueT>
inline void MapNode<KeyT, ValueT>::GetAll(List<ValueT*>* vec)
{
	vec->Add(&Value);
	if (RVal != nullptr)
		RVal->GetAll(vec);
	if (LVal != nullptr)
		LVal->GetAll(vec);
}

template<class KeyT, class ValueT>
inline bool MapNode<KeyT, ValueT>::TryGet(const KeyT& key, ValueT* value)
{
	if (Key == key)
	{
		*value = Value;
		return true;
	}
	else
	{
		return 
			Key < key 
				? (RVal != nullptr && RVal->TryGet(key, value)) 
				: (LVal != nullptr && LVal->TryGet(key, value));
	}
}

template<class KeyT, class ValueT>
inline bool MapNode<KeyT, ValueT>::TryAdd(const KeyT& key, const ValueT& value)
{
	if (Key == key)
	{
		return false;
	}
	else if (Key < key)
	{
		if (RVal == nullptr)
		{
			RVal = new MapNode<KeyT, ValueT>(key, value);
			return true;
		}
		else
		{
			return RVal->TryAdd(key, value);
		}
	}
	else // (Key > key)
	{
		if (LVal == nullptr)
		{
			LVal = new MapNode<KeyT, ValueT>(key, value);
			return true;
		}
		else
		{
			return LVal->TryAdd(key, value);
		}
	}
}

template<class KeyT, class ValueT>
inline Map<KeyT, ValueT>::Map() :
	root(nullptr)
{
}

template<class KeyT, class ValueT>
inline Map<KeyT, ValueT>::~Map()
{
	delete root;
}

template<class KeyT, class ValueT>
inline bool Map<KeyT, ValueT>::ContainsKey(const KeyT& key)
{
	return root != nullptr && root->ContainsKey(key);
}

template<class KeyT, class ValueT>
inline bool Map<KeyT, ValueT>::ContainsValue(const ValueT& value)
{
	return root != nullptr && root->ContainsValue(value);
}

template<class KeyT, class ValueT>
inline void Map<KeyT, ValueT>::GetAll(List<ValueT*>* vec)
{
	if (root != nullptr)
		root->GetAll(vec);
}

template<class KeyT, class ValueT>
inline bool Map<KeyT, ValueT>::TryGet(const KeyT& key, ValueT* value)
{
	return root != nullptr && root->TryGet(key, value);
}

template<class KeyT, class ValueT>
inline bool Map<KeyT, ValueT>::TryAdd(const KeyT& key, const ValueT& value)
{
	bool ret;
	if (root == nullptr)
	{
		root = new MapNode<KeyT, ValueT>(key, value);
		ret = true;
	}
	else
	{
		ret = root->TryAdd(key, value);
	}
	return ret;
}
