import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PopupeventidqueryComponent } from './popupeventidquery.component';

describe('PopupeventidqueryComponent', () => {
  let component: PopupeventidqueryComponent;
  let fixture: ComponentFixture<PopupeventidqueryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PopupeventidqueryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PopupeventidqueryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
