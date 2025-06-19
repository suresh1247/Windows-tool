import { Component, inject, OnInit } from '@angular/core';
import { HttpsService } from '../https.service';
import { ResultsDataService } from '../results-data.service';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-results-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './results-table.component.html',
  styleUrl: './results-table.component.css'
})
export class ResultsTableComponent implements OnInit {
home() {
  alert("HOme")
this.router.navigate(['']);
}
  public ResultsMap = new Map<string, any>();

  public HoverColumnNames: Set<string> = new Set();
  public columnNames: Set<string> = new Set();
  public hoveredServer: string | null = null;
  public hoverColumn: string | null = null;
  public hoverData: any[] = [];

  constructor(private httpservice: HttpsService,public dataservice:ResultsDataService,public router:Router) { }

  ngOnInit() {
    this.dataservice.ListServers.forEach(element => {
      
      this.ResultsMap.set(element, "Loading");
      this.httpservice.Onserver(element, this.dataservice.functionname).subscribe(response => {
  // alert("response "+JSON.stringify(response));
  this.ResultsMap.set(element, response);
  Object.keys(response).forEach(key => {
    if (key.endsWith("Hover")) {
      this.HoverColumnNames.add(key);
    } else {
      this.columnNames.add(key);
    }
  });
});
    });
    // alert(JSON.stringify(this.dataservice));
  }
  objectstring(obj:any){
    return JSON.stringify(obj);
  }

  onHover(server: string, column: string) {
    if (this.HoverColumnNames.has(column + "Hover")) {
      this.hoveredServer = server;
      this.hoverColumn = column;
      this.hoverData = this.ResultsMap.get(server)[column + "Hover"];
    }
  }

  onLeave() {
    this.hoveredServer = null;
    this.hoverColumn = null;
    this.hoverData = [];
  }
   isObject(value: any): boolean {
    return value && typeof value === 'object' && !Array.isArray(value);
  }
}