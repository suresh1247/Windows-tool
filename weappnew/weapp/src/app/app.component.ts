import { Component } from '@angular/core';
import { NavComponent } from './nav/nav.component';
import { RouterModule } from '@angular/router';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [NavComponent,RouterModule],  
})
export class AppComponent {
  title(title: any) {
    throw new Error('Method not implemented.');
  }
  
}