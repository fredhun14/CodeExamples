package com.example.test;

import androidx.appcompat.app.AppCompatActivity;
import android.app.Activity;
import android.database.Cursor;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.ActionMode;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.os.Bundle;
import android.text.InputFilter;
import android.content.Intent;
import android.widget.Toast;
import android.view.ActionMode.Callback;
import android.view.Menu;
import android.view.MenuItem;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.TimeZone;
import java.math.*;
import java.lang.*;

public class MainActivity extends AppCompatActivity {
    Connection connect;
    String ConnectionResult = "";
    boolean hasSerial = false;
    DatabaseHelper mDatabaseHelper;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        mDatabaseHelper = new DatabaseHelper(this);
        TimeZone tz = TimeZone.getTimeZone("PST");

        Button scanlognavbutton = (Button) findViewById(R.id.ScanLogNavButton);
        scanlognavbutton.setOnClickListener(new View.OnClickListener(){
         @Override
         public void onClick(View v){
            Intent intent = new Intent (getBaseContext(), ScanLog.class);
            startActivity(intent);
         }
        });


        EditText Scan = (EditText) findViewById((R.id.Scan));
        Scan.setShowSoftInputOnFocus(false);
        Scan.setLongClickable(false);
        Scan.setTextIsSelectable(false);
        Scan.requestFocus();
        Scan.setCustomSelectionActionModeCallback(new ActionMode.Callback() {

            public boolean onCreateActionMode(ActionMode actionMode, Menu menu) {
                return false;
            }

            public boolean onPrepareActionMode(ActionMode actionMode, Menu menu) {
                return false;
            }

            public boolean onActionItemClicked(ActionMode actionMode, MenuItem item) {
                return false;
            }

            public void onDestroyActionMode(ActionMode actionMode) {
            }
        });

        Scan.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence charSequence, int i, int i1, int i2) {

            }

            @Override
            public void onTextChanged(CharSequence charSequence, int i, int i1, int i2) {
                TextView Serial = (TextView) findViewById((R.id.LastSerialScanned));
                TextView Location = (TextView) findViewById((R.id.LastLocationScanned));
                if(hasSerial == false)
                {
                    if(Scan.getText().length() == 8)
                    {
                        boolean badscan = false;
                        char[] scanchar = new char[Scan.getText().length()];
                        for(int x = 0; x < scanchar.length; x++)
                        {
                            scanchar[x] = Scan.getText().charAt(i);
                            if(scanchar[x]< 48 || scanchar[x] > 57){
                                badscan=true;
                            }
                        }
                        if(!badscan) {
                            Serial.setText(Scan.getText());
                            hasSerial = true;
                            Location.setText("");
                            Scan.setText("");
                            int maxLength = 4;
                            InputFilter[] fArray = new InputFilter[1];
                            fArray[0] = new InputFilter.LengthFilter(maxLength);
                            Scan.setFilters(fArray);
                        }else{
                            Scan.setText("");
                            toastMessage("Invalid serial scan!");
                        }
                    }
                }
                if(hasSerial == true && Scan.getText().length() > 1)
                {
                    boolean badscan = true;
                    char[] scanchar = new char[Scan.getText().length()];
                    for(int x = 0; x <= Scan.getText().length(); x++)
                    {
                        scanchar[x] = Scan.getText().charAt(i);
                        if(scanchar[x]> 48 && scanchar[x] < 57){
                            badscan=false;
                            break;
                        }   else{badscan=true;}
                        if(scanchar[x]> 65 && scanchar[x] < 90){
                            badscan=false;
                            break;
                        }   else{badscan=true;}
                        if(scanchar[x]> 97 && scanchar[x] < 122){
                            badscan=false;
                            break;
                        }   else{badscan=true;}
                        // if(scanchar[x] == ';'){
                        //   badscan=false;
                        // break;
                        //}   else{badscan=true;}
                    }
                    if(!badscan) {
                        Location.setText(Scan.getText());
                        hasSerial = false;
                        Scan.setText("");


                        int maxLength = 8;
                        InputFilter[] fArray = new InputFilter[1];
                        fArray[0] = new InputFilter.LengthFilter(maxLength);
                        Scan.setFilters(fArray);


                        if(Serial.length() != 0 && Location.length() != 0){
                            AddData(Serial.getText().toString(), Location.getText().toString());
                        }else{
                            toastMessage("Missing either location or serial when attempting to log location!");
                        }
                    }else{
                        Scan.setText("");
                        toastMessage("Invalid location scan!");
                    }
                }
            }

            @Override
            public void afterTextChanged(Editable editable) {

            }
        });
    }

    public void SendupdatestoSQL(View v)
    {
        try{
            mDatabaseHelper = new DatabaseHelper(this);
            ConnectionHelper connectionHelper = new ConnectionHelper();
            connect = connectionHelper.connectionclass();
            if(connect!=null)
            {
                Cursor data = mDatabaseHelper.getUnsentData();
                int UpCount= 0;
                int InCount= 0;
                while(data.moveToNext()) {
                    try {
                        if("NO".equals(data.getString(4))) {
                            String UpdateQuery = "UPDATE DBO.Location_Log SET Location_Current = '" + data.getString(2) +
                                    "', Date_Time = '" + data.getString(3) + "' WHERE SERIAL_NUMBER = '" + data.getString(1) + "'";
                            Statement stUp= connect.createStatement();
                            Integer rsUp= stUp.executeUpdate(UpdateQuery);
                            if(rsUp.equals(0)) {
                                String query = "INSERT INTO DBO.Location_Log (SERIAL_NUMBER, Location_Current, Date_Time) VALUES ('" + data.getString(1) + "', '" + data.getString(2) + "', '" + data.getString(3) + "')";
                                Statement st = connect.createStatement();
                                st.executeUpdate(query);
                                InCount = InCount + 1;
                            }else{
                                UpCount = UpCount + 1;
                            }

                            //Below for updateing local
                            int itemID = data.getInt(0);
                            if (itemID > -1) {
                                Log.d("On Sync:", " The ID is: " + itemID);
                                mDatabaseHelper.UpdateSent("YES", itemID, data.getString(4));
                            } else {
                                toastMessage("failed to update local data once.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.e("ERROR", ex.getMessage() + ex.getCause());
                    }
                   // toastMessage(rs.toString());
                }
                toastMessage("Updated: " + UpCount + " locations!\nInserted: " + InCount + "locations!" );
            }
            else
            {
                ConnectionResult="Check Connection";
            }
        }
        catch (Exception ex)
        {
            Log.e("ERROR", ex.getMessage());
        }


    }
    public void AddData(String newSerial, String newLocation){
        boolean insertData = mDatabaseHelper.addData(newSerial,newLocation);
        if (insertData){
            toastMessage("Data Successfully Inserted!");
        }else{
            toastMessage("Something went wrong!");
        }
    }

    private void toastMessage(String message){
        Toast.makeText(this,message,Toast.LENGTH_SHORT).show();
    }

    public void ResetSent(View V)
    {
        mDatabaseHelper = new DatabaseHelper(this);
        Cursor data = mDatabaseHelper.getSentData();
        while(data.moveToNext()) {
            int itemID = data.getInt(0);
            mDatabaseHelper.UpdateSent("NO", itemID, data.getString(4));
        }
    }
    public void ResetScan(View V)
    {
        hasSerial = false;
        TextView Serial = (TextView) findViewById((R.id.LastSerialScanned));
        TextView Location = (TextView) findViewById((R.id.LastLocationScanned));
        EditText Scan = (EditText) findViewById((R.id.Scan));
        Serial.setText("");
        Location.setText("");
        Scan.setText("");
        int maxLength = 8;
        InputFilter[] fArray = new InputFilter[1];
        fArray[0] = new InputFilter.LengthFilter(maxLength);
        Scan.setFilters(fArray);
    }
}