package com.example.test;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.content.Context;
import com.google.android.material.tabs.TabLayout;
import android.util.Log;

public class DatabaseHelper extends SQLiteOpenHelper {
    private static final String TAG = "DatabaseHelper";

    private static final String TABLE_NAME = "Location_Log";
    private static final String Col0 = "ID";
    private static final String Col1 = "Serial";
    private static final String Col2 = "Location";
    private static final String Col3 = "Time_Date";
    private static final String Col4 = "Sent";
    public DatabaseHelper(Context context){
        super(context, TABLE_NAME, null, 1);

    }

    @Override
    public void onCreate(SQLiteDatabase db){
        String createTable = "CREATE TABLE " + TABLE_NAME + " (ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                Col1 + " TEXT, " + Col2 + " Text, " + Col3 + " DATETIME DEFAULT (datetime('now','localtime')), " + Col4 + " TEXT)";
        db.execSQL(createTable);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int i, int i1){
        db.execSQL("DROP IF TABLE EXSITS " + TABLE_NAME);
        onCreate(db);
    }
    public boolean addData(String serial, String location){
        SQLiteDatabase db = this.getWritableDatabase();
        ContentValues contentValues = new ContentValues();
        contentValues.put(Col1, serial);
        contentValues.put(Col2, location);
        contentValues.put(Col4, "NO");
        Log.d(TAG, "addData: Adding " + serial + " and " + location  + " to " + TABLE_NAME);
        long result = db.insert(TABLE_NAME, null,contentValues);

        if(result == -1){
            return false;
        }else{
            return true;
        }
    }
    public Cursor getData(){
        SQLiteDatabase db = this.getWritableDatabase();
        String query = "SELECT * FROM " + TABLE_NAME + " ORDER BY ID DESC LIMIT 10";
        Cursor data = db.rawQuery(query,null);
        return data;
    }
    public Cursor getUnsentData(){
        SQLiteDatabase db = this.getWritableDatabase();
        String query = "SELECT * FROM " + TABLE_NAME + " WHERE Sent='NO'";
        Cursor data = db.rawQuery(query,null);
        return data;
    }
    public Cursor getSentData(){
        SQLiteDatabase db = this.getWritableDatabase();
        String query = "SELECT * FROM " + TABLE_NAME + " WHERE Sent='YES'";
        Cursor data = db.rawQuery(query,null);
        return data;
    }
public Cursor getItemID(String serial){
        SQLiteDatabase db = this.getWritableDatabase();
        String query = "SELECT " + Col1 + " FROM " + TABLE_NAME +
                " WHERE " + Col2 + " = '" + serial + "'";
        Cursor data = db.rawQuery(query, null);
        return data;
}
public void UpdateSent(String status, int id, String oldstatus){
        SQLiteDatabase db = this.getWritableDatabase();
        String query = "UPDATE " + TABLE_NAME + " SET " + Col4 +
                " = '" + status + "' WHERE " + Col0 + " = '" + id + "' "
                + " AND " + Col4 + " = '" + oldstatus + "'";
        Log.d(TAG, "UpdateSent: query: " + query);
        Log.d(TAG, "UpdateSent: setting sent to " + status);
        db.execSQL(query);
}


}
