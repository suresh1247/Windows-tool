import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ResultsTableComponent } from './results-table/results-table.component';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
    {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'results',
    component: ResultsTableComponent,
  }
];
