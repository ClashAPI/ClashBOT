import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPluginsClashapiComponent } from './dashboard-plugins-clashapi.component';

describe('DashboardPluginsClashapiComponent', () => {
  let component: DashboardPluginsClashapiComponent;
  let fixture: ComponentFixture<DashboardPluginsClashapiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardPluginsClashapiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardPluginsClashapiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
