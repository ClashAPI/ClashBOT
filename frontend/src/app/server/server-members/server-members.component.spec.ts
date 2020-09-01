import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerMembersComponent } from './server-members.component';

describe('ServerMembersComponent', () => {
  let component: ServerMembersComponent;
  let fixture: ComponentFixture<ServerMembersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServerMembersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServerMembersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
