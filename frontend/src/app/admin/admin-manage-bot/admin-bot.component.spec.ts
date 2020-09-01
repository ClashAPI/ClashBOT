import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationManageBotComponent } from './administration-manage-bot.component';

describe('AdministrationManageBotComponent', () => {
  let component: AdministrationManageBotComponent;
  let fixture: ComponentFixture<AdministrationManageBotComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationManageBotComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationManageBotComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
