package com.example.test;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.database.Cursor;
import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListAdapter;
import android.widget.ListView;
import android.widget.Toast;
import android.util.Log;
import java.util.ArrayList;
import android.widget.TextView;
import android.view.ViewGroup;
import android.util.TypedValue;

public class ScanLog extends AppCompatActivity {

    private static final String TAG = "ListDataActivity";
    DatabaseHelper mDatabaseHelper;
    private ListView mListView;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_scan_log);
        mListView = (ListView) findViewById(R.id.listView);
        mDatabaseHelper = new DatabaseHelper(this);

        populateListView();


        Button scannavbutton = (Button) findViewById(R.id.ScanNavButton);
        scannavbutton.setOnClickListener(new View.OnClickListener(){
            @Override
            public void onClick(View v){
                Intent intent = new Intent (getBaseContext(), MainActivity.class);
                startActivity(intent);

            }
        });
    }
    private void populateListView(){
        Log.d(TAG, "populateListView: Displaying data in the ListView.");

        Cursor data = mDatabaseHelper.getData();
        ArrayList<String> listData = new ArrayList<>();
        while(data.moveToNext()){
            listData.add("Serial: " + data.getString(1) + "\n" + "Location: " + data.getString(2) + "\n" + data.getString(3) + "\nSynced: " + data.getString(4));
            //listData.add("Location: " + data.getString(2));
            //listData.add(data.getString(3));
        }

        ListAdapter adapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, listData) {
            @Override
            public View getView(int position, View convertView, ViewGroup parent) {

                TextView item = (TextView) super.getView(position, convertView, parent);
                // Change the item text size
                item.setTextSize(TypedValue.COMPLEX_UNIT_DIP, 30);
                return item;
            }
        };
        mListView.setAdapter(adapter);
    }

    private void toastMessage(String message){
        Toast.makeText( this,message,Toast.LENGTH_SHORT).show();
    }
}