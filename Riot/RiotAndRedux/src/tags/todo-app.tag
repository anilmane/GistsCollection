<todo-app>
	<loading-indicator loading={this.state.isLoading}></loading-indicator>
	<task-form addtask={this.handleNewTask}></task-form>
	<tasks-list tasks={this.state.tasks} handlecheck={handleTaskCompletionChange}></tasks-list>

	<script>
		var actions = require('../actions.js');
		var store = this.opts.store;

		this.on('mount', function() {
			store.dispatch(actions.loadTasks());
		});

		store.subscribe(function() {
			this.state = store.getState();
			this.update();
		}.bind(this));

		handleNewTask(task) {
			store.dispatch(actions.addTask(task));
		}

		handleTaskCompletionChange(id, isComplete) {
			store.dispatch(actions.toggleComplete(id, isComplete));
		}
	</script>
</todo-app>