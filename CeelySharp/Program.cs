using CeelySharp.Maybe;
using CeelySharp.MList;

var c = new MList<int> { 7, 8, 9 };
// var d = new MList<int> { 1, 4, 9 };
// var e = new MList<int>();

// var p = ProductAsync(a, b);

// var p = Product4Async(a, b, c, d);

// var p = Unit("asd");

var a = new MList<int>{ 1, 2, 3 };
var b = new MList<int> { 4, 5, 6 };
var p = Product(a, b);

foreach (var t in p)
{
    Console.WriteLine(t);
}

async MList<(T, TU)> ProductAsync<T, TU>(MList<T> ts, MList<TU> us)
{
    var t = await ts;
    var u = await us;
    return (t, u);
}

MList<(int, TU)> Product<TU>(MList<int> ts, MList<TU> us) =>
    from t in ts
    from u in us
    where t == 2
    select (t, u);

async MList<(T, TU, TV, TW)> Product4Async<T, TU, TV, TW>(MList<T> ts, MList<TU> us, MList<TV> vs, MList<TW> ws)
{
    var t = await ts;
    var u = await us;
    var v=  await vs;
    var w = await ws;
    return (t, u, v, w);
}

async MList<T> Unit<T>(T t)
{
    return t;
}

async Maybe<(T, TU)> ProductAsync2<T, TU>(Maybe<T> ts, Maybe<TU> us)
{
    var t = await ts;
    var u = await us;
    return (t, u);
}

Maybe<(int, TU)> Product2<TU>(Maybe<int> ts, Maybe<TU> us) =>
    from t in ts
    from u in us
    where t == 2
    select (t, u);

async Maybe<T> Unit2<T>(T t)
{
    return t;
}
