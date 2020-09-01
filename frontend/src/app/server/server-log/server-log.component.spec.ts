import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerLogComponent } from './server-log.component';

describe('ServerLogComponent', () => {
  let component: ServerLogComponent;
  let fixture: ComponentFixture<ServerLogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServerLogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServerLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
