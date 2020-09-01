import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GuildSelectorComponent } from './guild-selector.component';

describe('GuildSelectorComponent', () => {
  let component: GuildSelectorComponent;
  let fixture: ComponentFixture<GuildSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GuildSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GuildSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
