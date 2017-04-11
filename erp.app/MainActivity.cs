using Android.App;
using Android.OS;
using Android.Widget;

namespace erp.app
{
    [Activity(Label = "SQLite测试", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            //注册控件
            EditText txt_writeKey = FindViewById<EditText>(Resource.Id.txt_WriteKey);
            EditText txt_writeValue = FindViewById<EditText>(Resource.Id.txt_WriteValue);
            EditText txt_ReadKey = FindViewById<EditText>(Resource.Id.txt_ReadKey);
            Button btn_Write = FindViewById<Button>(Resource.Id.btn_Write);
            Button btn_Read = FindViewById<Button>(Resource.Id.btn_Read);
            //点击按钮
            btn_Write.Click += (o, e) =>
            {
                if (txt_writeKey.Text == "")
                {
                    Toast.MakeText(this, "请输入要写入的Key", ToastLength.Short).Show();
                    return;
                }
                if (txt_writeValue.Text == "")
                {
                    Toast.MakeText(this, "请输入要写入的Value", ToastLength.Short).Show();
                    return;
                }
                CreateTable();
                int count = 0;
                count = SqliteHelper.ExecuteScalarNum("select count(key) from test where key=@key",
                    "@key=" + txt_writeKey.Text);
                if (count > 0)
                {
                    Toast.MakeText(this, "Key:" + txt_writeKey.Text + " 已存在，不允许重复", ToastLength.Short).Show();
                    return;
                }
                count = SqliteHelper.ExecuteNonQuery("insert into test(key,value)values (@key,@value)",
                    "@key=" + txt_writeKey.Text,
                    "@value=" + txt_writeValue.Text);
                if (count > 0)
                {
                    txt_writeKey.Text = "";
                    txt_writeValue.Text = "";
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("提示：");
                    builder.SetMessage("写入成功\n");
                    builder.SetPositiveButton("确定", delegate { });
                    builder.SetNegativeButton("取消", delegate { });
                    builder.Show();
                }
                else
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("提示：");
                    builder.SetMessage("写入失败\n" + SqliteHelper.SqlErr);
                    builder.SetPositiveButton("确定", delegate { });
                    builder.SetNegativeButton("取消", delegate { });
                    builder.Show();
                }
            };
            btn_Read.Click += (o, e) =>
            {
                if (txt_ReadKey.Text == "")
                {
                    Toast.MakeText(this, "请输入要读取的Key", ToastLength.Short).Show();
                    return;
                }
                string value = SqliteHelper.ExecuteScalar("select value from test where key=@key",
                    "@key=" + txt_ReadKey.Text);
                if (string.IsNullOrEmpty(value))
                {
                    Toast.MakeText(this, "Key:" + txt_ReadKey.Text + " 不存在，清先写入该Key", ToastLength.Short).Show();
                    return;
                }
                txt_ReadKey.Text = "";
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("提示：");
                builder.SetMessage("Key:" + txt_ReadKey.Text + " 对应的value为\n" + value);
                builder.SetPositiveButton("确定", delegate { });
                builder.SetNegativeButton("取消", delegate { });
                builder.Show();
            };
        }
        /// <summary>
        /// 创建表
        /// </summary>
        private void CreateTable()
        {
            if (!SqliteHelper.of_ExistTable("test"))
            {
                string ls_sql = @"CREATE TABLE test ( 
                                    key  VARCHAR(50) NOT NULL,
                                    value  VARCHAR(50) NOT NULL,
                                    primary key (key,value) 
                                    );  ";
                SqliteHelper.ExecuteNonQuery(ls_sql);
            }
        }
    }
}

