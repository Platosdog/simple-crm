import { StatusIconPipe } from './status-icon.pipe';

describe('StatusIconPipe', () => {
  it('create an instance', () => {
    const pipe = new StatusIconPipe();
    expect(pipe).toBeTruthy();
  });
});
it('Prospect should result in online', () => {
  const pipe = new StatusIconPipe(); // 1. SETUP: construct a new instance of the class.
  const x = pipe.transform('Prospect'); // 2. INVOKE the method
  expect(x).toEqual('online'); // 3. VERIFY the result of the method matches what is expected.
});
