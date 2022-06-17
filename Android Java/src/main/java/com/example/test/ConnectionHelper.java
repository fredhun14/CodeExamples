package com.example.test;

import java.sql.Connection;
import java.sql.DriverManager;

import android.annotation.SuppressLint;
import android.os.StrictMode;
import android.util.Log;
import android.database.*;

public class ConnectionHelper {
    Connection con;
    String uname, pass, ip, port, database;

    public Connection connectionclass()
    {
        ip = "10.10.10.38";
        database = "SFF_Locations";
        uname = "software";
        pass = "!Mk!03625";
        port = "1433";
        @SuppressLint("NewApi")
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);
        Connection connection = null;
        String ConnectionURL = null;
        try
        {
            Class.forName("net.sourceforge.jtds.jdbc.Driver");
            ConnectionURL= "jdbc:jtds:sqlserver://"+ ip + ";"+ "databasename="+ database+";user="+uname+";password="+pass+";";
            connection = DriverManager.getConnection(ConnectionURL);
        }
        catch (SQLException se) {
            Log.e("ERROR", se.getMessage());
        } catch (ClassNotFoundException e) {
            Log.e("ERROR", e.getMessage());
        } catch (Exception e) {
            Log.e("ERROR", e.getMessage());
        }
        return connection;
    }
}
