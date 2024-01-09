using Firebase.Database;
using Firebase.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Rapper
{
    public string name;
    public int age;
    public string rapName;
    public List<string> rival;
}

public class DatabaseAhuhu : MonoBehaviour
{
    [SerializeField] Rapper rapper, rapperLoad;
    DatabaseReference databaseReference;

    private void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Ngoài ValueChanged ra còn có ChildAdded, ChildChanged, ChildRemoved, ChildMoved
        databaseReference.Child("quacquac").ValueChanged += HandleValueChanged;
    }

    public void SaveData()
    {
        //// Lưu dữ liệu rapper thành con của 1 key ngẫu nhiên tự sinh (hàm Push() chính là tạo key)
        //databaseReference.Push().SetRawJsonValueAsync(JsonConvert.SerializeObject(rapper)).ContinueWithOnMainThread(task =>
        //{
        //    print("Đã lưu data thành công");
        //});

        // Lưu dữ liệu rapper thành con của "quacquac"
        databaseReference.Child("quacquac").SetRawJsonValueAsync(JsonConvert.SerializeObject(rapper)).ContinueWithOnMainThread(task =>
        {
            print("Đã lưu data thành công");
        });
    }

    public void LoadDataAhuhu()
    {
        // Thêm .LimitToFirst(100).OrderBy(...) nếu muốn tối ưu, chỉ lấy 100 tk phần tử đầu tiên theo rule nào đó
        databaseReference.Child("quacquac").GetValueAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsFaulted)
            {
                print("Load bị lỗi cmnr");
            }
            else if (task.IsCompleted)
            {
                rapperLoad = JsonConvert.DeserializeObject<Rapper>(task.Result.GetRawJsonValue());
                print("Đã load thành công");
            }
        });
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        print("Dữ liệu đã thay đổi");
        LoadDataAhuhu();
    }
}
