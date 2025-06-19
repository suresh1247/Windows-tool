import { Component, Output, EventEmitter, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, of } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule } from '@angular/material/core';
import { HttpsService } from '../https.service';
import { ResultsDataService } from '../results-data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'event-id-query',
  templateUrl: './popupeventidquery.component.html',
  styleUrls: ['./popupeventidquery.component.css'],
  imports: [FormsModule, NgIf,MatDatepickerModule,MatInputModule,MatNativeDateModule,]
})
export class PopupeventidqueryComponent {
  LogName: string = "";            
  EventIDs: string="";         
  StartDate: Date | null = null;   
  EndDate: Date | null = null;     
  EventType: string = "";          
  EventSource: string = "";        
  useCurrentDate: boolean = false;
  responseMessage=""
  resultMessage = '';
  showDialog = false;


  constructor(private data:ResultsDataService,private router:Router) {}

  openDialog() {
    this.showDialog = true;
  }

  closeDialog() {
    this.showDialog = false;
  }

  updateDates() {
  if (this.useCurrentDate) {
    const currentDate = new Date();
    this.StartDate = currentDate;
    this.EndDate = currentDate;
  } else {
    this.StartDate = null;
    this.EndDate = null;
  }
}

onDateChange(type: string, event: any) {
  const selectedDate = new Date(event.value);
  if (type === 'start') {
    this.StartDate = selectedDate;
  } else if (type === 'end') {
    this.EndDate = selectedDate;
  }
}

// Optionally, formatDateWithTime can be retained for displaying purposes or when converting to a string for UI purposes:
formatDateWithTime(date: Date): string {
  const mm = String(date.getMonth() + 1).padStart(2, '0');
  const dd = String(date.getDate()).padStart(2, '0');
  const yyyy = date.getFullYear();
  const hh = String(date.getHours()).padStart(2, '0');
  const min = String(date.getMinutes()).padStart(2, '0');
  const ss = String(date.getSeconds()).padStart(2, '0');
  return `${mm}/${dd}/${yyyy} ${hh}:${min}:${ss}`;
}

  // formatDate(date: Date): string {
  //   const mm = String(date.getMonth() + 1).padStart(2, '0');
  //   const dd = String(date.getDate()).padStart(2, '0');
  //   const yyyy = date.getFullYear();
  //   const hh = String(date.getHours()).padStart(2, '0');
  //   const min = String(date.getMinutes()).padStart(2, '0');
  //   const ss = String(date.getSeconds()).padStart(2, '0');
  //   return `${mm}/${dd}/${yyyy} ${hh}:${min}:${ss}`;
  // }

  openDatePicker(type: string) {
    // Logic to open a date picker will be added here
    // For now, you can use a date picker library to handle date selection
  }

  submitRequest() {
    const url = 'http://localhost:5168/Server/query';
    const requestPayload = {
      // ComputerName: this.servername,
      EventIDs: this.EventIDs ? this.EventIDs.split(',').map(id => id.trim()) : [], // Split and trim event IDs string
      LogName: this.LogName,
      StartDate: this.StartDate ? new Date(this.StartDate).toISOString() : null, // Convert to ISO string
      EndDate: this.EndDate ? new Date(this.EndDate).toISOString() : null, // Convert to ISO string
      EventType: this.EventType,
      EventSource: this.EventSource
    };
    console.log('Request Payload:', requestPayload);
    var res="EventId"+JSON.stringify(requestPayload)
    this.data.name=res;
    this.data.functionname="server/query";
    this.showDialog = false;
    this.router.navigate(["results"])
  }
}