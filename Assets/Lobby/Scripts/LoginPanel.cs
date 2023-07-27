using ExitGames.Client.Photon;
using MySql.Data.MySqlClient;
using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField passInputField;

    private MySqlConnection con;
    private MySqlDataReader reader;

    private void Start()
    {
        ConnectDataBase();
    }

    private void ConnectDataBase()
    {
        try
        {
            string serverInfo = "Server=43.202.3.31; Database=userdata; Uid=root; Pwd=1234; Port=3306; CharSet=utf8";
            con = new MySqlConnection(serverInfo);
            con.Open();

            // 성공
            Debug.Log("DataBase connect success");
        }
        catch (InvalidCastException ex) 
        {
            // 접속은 되긴했으나, 캐스트에 실패했을때
            Debug.Log(ex.Message);
        }
        catch (Exception ex)
        {
            // 접속 자체가 안됐을 때
            Debug.Log(ex.Message);
        }
    }

    public void Login()
    {
        try
        {
            string id = idInputField.text;
            string pass = passInputField.text;

            string sqlCommand = string.Format("SELECT ID,PWD FROM user_info WHERE ID='{0}'", id);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string readID = reader["ID"].ToString();
                    string readPass = reader["PWD"].ToString();

                    Debug.Log($"ID : {readID}, PWD : {readPass}");

                    if (pass == readPass)
                    {
                        PhotonNetwork.LocalPlayer.NickName = idInputField.text;
                        PhotonNetwork.ConnectUsingSettings();
                        if (!reader.IsClosed)
                            reader.Close();
                        return;
                    }
                    else
                    {
                        Debug.Log("Wrong Password");
                        if (!reader.IsClosed)
                            reader.Close();
                    }
                }
            }
            else
            {
                Debug.Log("There is no playerID");
            }
            if (!reader.IsClosed)
                reader.Close();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        //if (idInputField.text == "")
        //{
        //    Debug.Log("Invalid Player ID");
        //    return;
        //}

        //PhotonNetwork.LocalPlayer.NickName = idInputField.text;
        //PhotonNetwork.ConnectUsingSettings();
        if (reader != null || !reader.IsClosed)
            reader.Close();
    }
}
