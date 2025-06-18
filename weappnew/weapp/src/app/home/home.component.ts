import { CommonModule, NgFor, NgIf } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { HttpsService } from '../https.service';
import { ResultsDataService } from '../results-data.service';
import { ServiceRestartComponent } from "../service-restart/service-restart.component";
import { PatchComponent } from "../patchwithkbn/patchwithkbn.component";
import { PopupeventidqueryComponent } from "../popupeventidquery/popupeventidquery.component";
interface ServerInfo {
  name: string;
  selected: boolean;
  pingStatus: string;
}
@Component({
  selector: 'app-home',
  imports: [FormsModule, RouterModule, ServiceRestartComponent, PatchComponent, PopupeventidqueryComponent,CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})

export class HomeComponent {

servers = "";
  serverList: Set<ServerInfo> = new Set<ServerInfo>();
  AddedServers=new Set<string>();

  private httpService = inject(HttpsService);
  constructor(private router:Router){

  }
  ngOnInit(){
    this.servicename.ListServers.clear();
  }
  public servicename=inject(ResultsDataService);
  onAddServers() {
    const serverArray = this.servers.split("\n");
    serverArray.forEach(server => {

      if(server.trim()!="" && !this.AddedServers.has(server.trim())){

      const element={name: server.trim(),
      selected: false,
      pingStatus: "Loading"
      }
      this.AddedServers.add(server.trim())
      this.httpService.Onserver(server,"server/ping").subscribe(response=>{
        element.pingStatus=response.res;
      })
      this.serverList.add(element);
    }
    });
  }
  
   logFunctionName(functionName: string): void {
    this.servicename.functionname=functionName;
    var arr=new Array<string>;
  
     this.router.navigate(['/results']);
  }
  updateSelectedServers(server:string){
    this.serverList.forEach(element => {
      if(element.selected){
      this.servicename.ListServers.add(element.name)
      }
      else{
        this.servicename.ListServers.delete(element.name);
      }
    });
  }

 
}
