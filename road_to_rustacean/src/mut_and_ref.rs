/// 使用
/// ```
/// mut obj: T
/// ```
/// 變數從函式外copy到函式內 <br/>
/// 函式內自己掌握新變數的所有權, 外部不變 <br/>
/// 函式內相當於宣告變數 <br/>
/// 外部不需要宣告 `mut` <br/>
/// ```
/// set_mut a: 3
/// set_mut b: 100
/// set_mut: 3
/// new_mut a: 3
/// new_mut b: 100
/// new_mut: 3
/// ```
pub fn demo_mut_copy() {
    // 有 copy trait 的進了函式發生甚麼事都跟外邊沒關係
    let foo_set = FooWithCopy{val:3};
    set_mut(foo_set);
    println!("set_mut: {}", foo_set.get());
    
    let foo_new = FooWithCopy{val:3};
    new_mut(foo_new);
    println!("new_mut: {}", foo_new.get());
}

/// 使用
/// ```
/// mut obj: T
/// ```
/// 變數從函式外移動到函式內 <br/>
/// 所有權都進到函式內 <br/>
/// 函式內相當於宣告變數 <br/>
/// 外部不需要宣告 `mut` <br/>
/// ```
/// set_mut a: 3
/// set_mut b: 100
/// new_mut a: 3
/// new_mut b: 100
/// ```
pub fn demo_mut_no_copy() {
    // 沒有 Copy, 物件被 move 進函式後後面就無法操作
    let foo_set = FooWithoutCopy{val:3};
    set_mut(foo_set);
    // error: borrow of moved value: `foo_set`
    // println!("set_mut: {}", foo_set.val);
    
    let foo_new = FooWithoutCopy{val:3};
    new_mut(foo_new);
    // error: borrow of moved value: `foo_new`
    // println!("new_mut: {}", foo_new.val);
}

fn set_mut<T: Foo>(mut obj: T) {
    println!("set_mut a: {}", obj.get());
    obj.set(100);
    println!("set_mut b: {}", obj.get());
}

fn new_mut<T: Foo>(mut obj: T) {
    println!("new_mut a: {}", obj.get());
    obj = T::new(100);
    println!("new_mut b: {}", obj.get());
}

/// 使用
/// ```
/// obj: &T
/// ```
/// 變數被參照進函式內 <br/>
/// 所有權還在函式外 <br/>
/// 函式內要解參照需要有 Copy trait, 解出時 copy 新變數<br/>
/// 外部需要宣告 `&` <br/>
/// ```
/// call_ref_copy: 3
/// call_ref_copy: 100
/// with_copy: 3
/// call_ref_no_copy: 100
/// no_copy: 100
/// ```
pub fn demo_call_ref() {
    // 帶 copy 的可以解參照, 但其實等於在函式內另做了新的物件, 不影響外邊物件
    let foo_call_ref = FooWithCopy{val:3};
    call_ref_copy(&foo_call_ref);
    println!("with_copy: {}", foo_call_ref.get());

    // 不能被解(沒有copy)
    // 不能被操作(型別沒有 mut)
    let foo_call_ref = FooWithoutCopy{val:100};
    call_ref_no_copy(&foo_call_ref);
    println!("no_copy: {}", foo_call_ref.get());
}

fn call_ref_copy<T: Foo + Copy>(obj: &T) {
    let mut oth = *obj;
    println!("call_ref_copy: {}", oth.get());
    oth.set(100);
    println!("call_ref_copy: {}", oth.get());
 }

fn call_ref_no_copy<T: Foo>(obj: &T) {
    // error: cannot move out of `*obj` which is behind a shared reference
    // let oth = *obj;

    // error: cannot borrow `*obj` as mutable, as it is behind a `&` reference
    // obj.set(100);
    println!("call_ref_no_copy: {}", obj.get());
}

/// 使用
/// ```
/// mut obj: &T
/// ```
/// 基本跟
/// ```
/// obj: &T
/// ```
/// 一樣, 主要差在 `obj` 在這區域變成可變了 <br/>
/// 另外, 雖然 `obj` 可變, 但如果 assign 一個在函式內建構的新實體 <br/>
/// 會由於在函式內建構的實體, 離開時就會消滅而報錯 <br/>
/// ```
/// call_mut_ref: 3
/// mut_ref: 3
/// ```
pub fn demo_call_mut_ref() {
    // 不能被 assign 在函式內創建的實體
    let mut foo_mut_ref = FooWithoutCopy{val:3};
    call_mut_ref(&foo_mut_ref);
    println!("mut_ref: {}", foo_mut_ref.get());
}

fn call_mut_ref<T: Foo>(mut obj: &T) {
    // error: temporary value dropped while borrowed
    // obj = &T::new(100);
    println!("call_mut_ref: {}", obj.get());
}


/// 使用
/// ```
/// obj: &mut T
/// ```
/// 變數參照傳進函式內, 而且是可變的類型
/// 或是說傳遞一個可變物件的參照, 但參照本身是不可變的
/// 也不能在函式內讓參照解出, 一樣是由 copy trait 決定
/// 但可以讓參照物件在函式內進行操作
/// ```
/// set_ref_mut a: 3
/// set_ref_mut b: 100
/// ref_mut: 100
/// new_ref_mut_no_copy a: 3
/// new_ref_mut_no_copy b: 100
/// ref_mut_no_copy: 100
/// call_ref_mut_copy a: 3
/// call_ref_mut_copy b: 100
/// ref_mut_copy: 3
/// ```
pub fn demo_ref_mut() {
    // 丟到函式內進行操作
    let mut foo_ref_mut = FooWithoutCopy{val:3};
    set_ref_mut(&mut foo_ref_mut);
    println!("ref_mut: {}", foo_ref_mut.get());

    // 在函式內可以 assign 新物件到參照並傳回來
    let mut foo_ref_mut_no_copy = FooWithoutCopy{val:3};
    new_ref_mut_no_copy(&mut foo_ref_mut_no_copy);
    println!("ref_mut_no_copy: {}", foo_ref_mut_no_copy.get());

    // 帶 copy 的解參照就等於在函式內另做了新的物件
    let mut foo_ref_mut_copy = FooWithCopy{val:3};
    call_ref_mut_copy(&mut foo_ref_mut_copy);
    println!("ref_mut_copy: {}", foo_ref_mut_copy.get());
}

fn set_ref_mut<T: Foo>(obj: &mut T) {
    println!("set_ref_mut a: {}", obj.get());
    obj.set(100);
    println!("set_ref_mut b: {}", obj.get());
}

fn new_ref_mut_no_copy<T: Foo>(obj: &mut T) {
    println!("new_ref_mut_no_copy a: {}", obj.get());
    *obj = T::new(100);
    println!("new_ref_mut_no_copy b: {}", obj.get());
}

fn call_ref_mut_copy<T: Foo + Copy>(obj: &mut T) {
    // error: cannot move out of `*obj` which is behind a mutable reference
    let mut oth = *obj;
    println!("call_ref_mut_copy a: {}", oth.get());
    oth.set(100);
    println!("call_ref_mut_copy b: {}", oth.get());
}

/* 以下定義 */

struct FooWithoutCopy {
    val: i32
}

struct FooWithCopy {
    val: i32
}

// 為 TypeWithCopy 實作 Copy
impl Clone for FooWithCopy {
    fn clone(&self) -> FooWithCopy { FooWithCopy { val: self.val } }
}

impl Copy for FooWithCopy {
}

// 共通的操作方法 for 兩個測試用物件
trait Foo {
    fn new(val: i32) -> Self;
    fn set(&mut self, val: i32);
    fn get(&self) -> i32;
}

impl Foo for FooWithCopy {
    fn new(val: i32) -> FooWithCopy { FooWithCopy { val: val } }
    fn set(&mut self, val: i32) { (*self).val = val }
    fn get(&self) -> i32 { self.val }
}

impl Foo for FooWithoutCopy {
    fn new(val: i32) -> FooWithoutCopy { FooWithoutCopy { val: val } }
    fn set(&mut self, val: i32) { (*self).val = val }
    fn get(&self) -> i32 { self.val }
}
