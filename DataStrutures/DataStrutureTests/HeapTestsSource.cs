using DataStrutures;
using System.Collections.Generic;
using static DataStrutureTests.HeapWithRefObjTests;

namespace DataStrutureTests;

public class HeapTestsSource
{
    public static object[] HeapTestsArgs = {
        new object[] { new Heap<int>(Comparer<int>.Default), 8 },
        new object[] { new HeapNode<int>(Comparer<int>.Default), 8 },
        new object[] { new Heap<int>(Comparer<int>.Default), 100 },
        new object[] { new HeapNode<int>(Comparer<int>.Default), 100 },
    };

    public static object[] HeapWithRefObjTestsArgs = {
        new object[] { new Heap<Foo>(new FooComparer()), 8 },
        new object[] { new HeapNode<Foo>(new FooComparer()), 8 },
        new object[] { new Heap<Foo>(new FooComparer()), 100 },
        new object[] { new HeapNode<Foo>(new FooComparer()), 100 },
    };
}