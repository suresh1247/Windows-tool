import { Component, Output, EventEmitter,Input, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, of } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { HttpsService } from '../https.service';
import { ResultsDataService } from '../results-data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-service-restart',
  templateUrl: './service-restart.component.html',
  styleUrls: ['./service-restart.component.css'],
  imports:[FormsModule,NgIf]
})
export class ServiceRestartComponent {
  serviceName:string="";
  servername!: string;
  showDialog = false;
  public dataservice=inject(ResultsDataService);
  constructor(private router:Router) {}


  openDialog() {
    this.showDialog = true;
  }
  

  closeDialog() {
    this.showDialog = false;
  }

  submitRequest() {
    this.dataservice.functionname="api/Restart/restart";
    
    this.dataservice.name=this.serviceName;
    this.showDialog = false;
    alert(this.serviceName);
    console.log(this.dataservice);
   this.router.navigate(['/results']);
  }
}