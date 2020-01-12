namespace BoardCore.Helpers{

    class Result<T,Err>{
        private T value;
        private Err error;

        public Result(T value,Err error){
            this.value =value;
            this.error=error;
        }

        public T Value{ get{ return value; } }
        public Err Error { get{return error;} }


    }
};