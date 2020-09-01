import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerRolesComponent } from './server-roles.component';

describe('ServerRolesComponent', () => {
  let component: ServerRolesComponent;
  let fixture: ComponentFixture<ServerRolesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServerRolesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServerRolesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
