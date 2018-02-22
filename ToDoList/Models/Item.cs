using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
  public class Item
  {
    private string _description;
    private int _categoryId;
    private int _id;

    public Item (string Description, int CategoryId = 0, int Id = 0)
    {
      _id = Id;
      _description = Description;
      _categoryId = CategoryId;
    }
    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
        Item newItem = (Item) otherItem;
        bool idEquality = (this.GetId() == newItem.GetId());
        bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
        bool categoryEquality = this.GetCategoryId() == newItem.GetCategoryId();
        return (idEquality && descriptionEquality);
      }
    }
    public override int GetHashCode()
    {
         return this.GetDescription().GetHashCode();
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public int GetId()
    {
      return _id;
    }
    public int GetCategoryId()
    {
        return _categoryId;
    }
    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        int itemCategoryId = rdr.GetInt32(2);

        Item newItem = new Item(itemDescription, itemCategoryId, itemId);
        allItems.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allItems;
    }
    public static Item Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `items` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int itemId = 0;
      string itemDescription = "";
      int itemCategoryId = 0;

      while (rdr.Read())
      {
          itemId = rdr.GetInt32(0);
          itemDescription = rdr.GetString(1);
          itemCategoryId = rdr.GetInt32(2);
      }

      Item foundItem= new Item(itemDescription, itemCategoryId, itemId);

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return foundItem;
    }
    public static void DeleteAll()
   {
     MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"DELETE FROM items;";

     cmd.ExecuteNonQuery();

     conn.Close();
     if (conn != null)
     {
         conn.Dispose();
     }
   }
   public void Delete()
  {
    MySqlConnection conn = DB.Connection();
    conn.Open();

    var cmd = conn.CreateCommand() as MySqlCommand;
    cmd.CommandText = @"DELETE FROM items WHERE id = @thisId;";

    MySqlParameter deleteId = new MySqlParameter();
    deleteId.ParameterName = "@thisId";
    deleteId.Value = _id;
    cmd.Parameters.Add(deleteId);

    cmd.ExecuteNonQuery();

    conn.Close();
    if (conn != null)
    {
        conn.Dispose();
    }
  }
   public void Save()
    {
      MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"INSERT INTO `items` (`description`,`category_id`) VALUES (@ItemDescription, @category_id);";

     MySqlParameter description = new MySqlParameter();
     description.ParameterName = "@ItemDescription";
     description.Value = this._description;
     cmd.Parameters.Add(description);

     MySqlParameter categoryId = new MySqlParameter();
     categoryId.ParameterName = "@category_id";
     categoryId.Value = this._categoryId;
     cmd.Parameters.Add(categoryId);

     cmd.ExecuteNonQuery();
     _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public void Edit(string newDescription)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@newDescription";
      description.Value = newDescription;
      cmd.Parameters.Add(description);

      cmd.ExecuteNonQuery();
      _description = newDescription;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
  }
}
