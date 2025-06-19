import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, of } from 'rxjs';
import { server } from 'typescript';
import { ResultsDataService } from './results-data.service';

@Injectable({
  providedIn: 'root'
})
export class HttpsService {
  private http = inject(HttpClient);
  private dataservice=inject(ResultsDataService);

  constructor() {}

  Onserver(server: string, url_part: string) {
    const url = 'http://localhost:5168/' + url_part;
    // alert(url)
    var data=null;
    if(this.dataservice.name!="" && !this.dataservice.name.startsWith("kbn")){
      data = { ComputerName: server, ServiceName: this.dataservice.name
       };
    }
    if(this.dataservice.name.startsWith("EventId")){
      data=JSON.parse(this.dataservice.name.substring(7));
      data.ComputerName=server;
    
    }
    if(this.dataservice.name.startsWith("kbn")){
      data = { server: server, kbNumber: this.dataservice.name.substring(3)
       };
    }
    console.log("data",data);
    // alert(JSON.stringify(data));

    return this.http.post<any>(url, JSON.stringify(data!=null?data: server), {
      headers: { 'Content-Type': 'application/json' }
    }).pipe(
      catchError(error => {
        // alert("error");
        console.error('Error pinging server:', error);
        return of({ Status: 'Error' });
      })
    );
  }
}