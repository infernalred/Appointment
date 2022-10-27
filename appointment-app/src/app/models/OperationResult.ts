export default interface OperationResult<T> {
  isSuccess: boolean;
  result: T;
  error?: string;
}
