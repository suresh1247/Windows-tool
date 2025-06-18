import { Component, Output, EventEmitter,Input, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, of } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { HttpsService } from '../https.service';
import { ResultsDataService } from '../results-data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-patch',
  templateUrl: './patchwithkbn.component.html',
  styleUrls: ['./patchwithkbn.component.css'],
  imports:[FormsModule,NgIf]
})
export class PatchComponent {
  kbnumber:string="";
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
    this.dataservice.functionname="api/patching/check";
    
    this.dataservice.name="kbn"+this.kbnumber;
    this.showDialog = false;
    console.log(this.dataservice);
   this.router.navigate(['/results']);
  }
}