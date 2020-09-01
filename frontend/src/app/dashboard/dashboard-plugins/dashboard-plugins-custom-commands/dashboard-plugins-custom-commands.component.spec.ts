import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardPluginsCustomCommandsComponent } from './dashboard-plugins-custom-commands.component';

describe('DashboardPluginsCustomCommandsComponent', () => {
  let component: DashboardPluginsCustomCommandsComponent;
  let fixture: ComponentFixture<DashboardPluginsCustomCommandsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardPluginsCustomCommandsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardPluginsCustomCommandsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
