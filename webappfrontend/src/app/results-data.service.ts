import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ResultsDataService {
  public functionname:string="";
  public ListServers=new Set<string>;
  public name="";
  public AllServers=new Set<string>;
}
