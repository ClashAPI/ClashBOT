import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerGuildComponent } from './server-guild.component';

describe('ServerGuildComponent', () => {
  let component: ServerGuildComponent;
  let fixture: ComponentFixture<ServerGuildComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServerGuildComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServerGuildComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
