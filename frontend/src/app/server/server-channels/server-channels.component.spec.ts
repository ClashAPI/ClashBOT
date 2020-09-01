import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServerChannelsComponent } from './server-channels.component';

describe('ServerChannelsComponent', () => {
  let component: ServerChannelsComponent;
  let fixture: ComponentFixture<ServerChannelsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServerChannelsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServerChannelsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
