use std::pin::Pin;

fn logic_join(left: Option<i32>, right: Option<i32>) {
    // 當兩者皆 Some, 回傳 right (and 內的值)
    left.and(right);

    // 回傳單獨為 Some 的值
    left.xor(right);

    // 回傳 left 或 right (left 優先)
    left.or(right);
}

fn predicate_and_next(opt: Option<i32>){
    let or_else: Option<i32> = opt.or_else(|| Some(3));

    let and_then: Option<i32> = opt.and_then(|x| Some(x * x));

    let map: Option<i32> = opt.map(|x| x * x);

    let map_or: i32 = opt.map_or(-1, |x| x * x);

    let map_or_else: i32 = opt.map_or_else(|| -1, |x| x * x);
}

/// 當使用 `as_mut` 或 `as_ref` 會把內部的變數以參照型式傳遞出來
/// 而 `as_mut` 差在參照為可變型別
/// ```
/// unwrap: 3
/// mov_unwrap: 3
/// as_mut: 3
/// as_mut_unwrap: 100
/// as_ref: 200
/// unwrap: 200
/// ```
pub fn convert_and_unwrap_option()
{
    let mut opt: Option<i32> = Some(3);

    // 從 option 取出物件, 除了 None 的 panic 外
    // 如果物件不具有 copy trait, 那物件就相當於移動了
    let mut unwrap: i32 = opt.unwrap();
    unwrap = 100;
    println!("unwrap: {}", opt.unwrap());
    let opt_2: Option<i32> = opt;
    println!("mov_unwrap: {}", opt_2.unwrap());

    // 這邊轉成可變參照後, 便可以用參照形式設定內部的值
    let as_mut: &mut i32 = (&mut opt).as_mut().unwrap();
    println!("as_mut: {}", as_mut);
    *as_mut = 100;
    println!("as_mut_unwrap: {}", opt.unwrap());

    // 注意的是如果又使用了 unwrap, 那原本的原始物件會被參照到另一個變數
    // 造成 mutably borrowed 錯誤
    // 如果具有 copy trait 可以再借一次參照
    // error: cannot use `opt` because it was mutably borrowed
    // *as_mut = 100;
    let as_mut_2: &mut i32 = opt.as_mut().unwrap();
    *as_mut_2 = 200;

    // as_ref 可以轉出參照, 但不能變化
    let as_ref: &i32 = opt.as_ref().unwrap();
    println!("as_ref: {}", as_ref);
    // error: cannot assign to `*as_ref`, which is behind a `&` reference
    // *as_ref = 300;

    println!("unwrap: {}", opt.unwrap());
}

/// box 物件把值 allocate 在 heap 上, 操作基本一樣
/// 更多意識在所有權跟借用的變化
/// ```
/// boxed: 3
/// as_mut: 3
/// as_ref: 100
/// ```
pub fn use_box ()
{
    let mut boxed: Box<i32> = Box::new(3);
    println!("boxed: {}",boxed);

    let as_mut: &mut i32= boxed.as_mut();
    println!("as_mut: {}",as_mut);
    *as_mut = 100;

    let as_ref: &i32 = boxed.as_ref();
    println!("as_ref: {}",as_ref);
}

/// pin 物件把值固定在 heap 的位置上, 操作上基本一樣
/// 更多意識在所有權跟借用的變化
/// ```
/// boxed: 3
/// as_mut: 100
/// get_mut: 100
/// boxed: 200
/// as_ref: 200
/// ```
pub fn use_box_and_pin ()
{
    let mut boxed: Pin<Box<i32>> =  Box::pin(3);
    println!("boxed: {}",boxed);

    let mut as_mut: Pin<&mut i32> = boxed.as_mut();
    *as_mut = 100;
    println!("as_mut: {}",as_mut);

    let get_mut: &mut i32 = as_mut.get_mut();
    println!("get_mut: {}",get_mut);
    *get_mut = 200;
    println!("boxed: {}",boxed);

    let as_ref: Pin<&i32> = boxed.as_ref();
    println!("as_ref: {}",as_ref);
}
